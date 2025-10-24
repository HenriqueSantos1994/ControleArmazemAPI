terraform {
  required_version = ">= 1.0.0"
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.15"
    }
  }
  backend "s3" {
    bucket = "tfstate-controleArmazem"
    key    = "api.tfstate"
    region = "us-east-1"
  }
}

provider "aws" {
  region = "us-east-1"
} // ...existing code...
resource "aws_iam_policy" "lambda_policy" {
  name        = "lambda-controleArmazem-policy"
  description = "Policy for Lambda to access DynamoDB"
  policy = jsonencode({
    Version = "2012-10-17",
    Statement : [
      {
        Action = [
          "dynamodb:PutItem",
          "dynamodb:GetItem",
          "dynamodb:UpdateItem",
          "dynamodb:DeleteItem",
          "dynamodb:Scan",
          "dynamodb:Query"
        ],
        Effect = "Allow",
        Resource = [
          aws_dynamodb_table.controle_armazem_table_Produtos.arn,
          aws_dynamodb_table.controle_armazem_table_Categoria.arn
        ]
      },
      {
        Action = [
          "logs:CreateLogGroup",
          "logs:CreateLogStream",
          "logs:PutLogEvents"
        ],
        Effect   = "Allow",
        Resource = "*"
      }
    ]
  })
}

resource "aws_lambda_function" "api_function" {
  function_name = "lambda_api_function"
  role          = aws_iam_role.lambda_execution_role.arn
  runtime       = "dotnet8"
  memory_size   = 512
  timeout       = 30
  #handler       = "ControleArmazem::ControleArmazem.LambdaEntryPoint::FunctionHandlerAsync"
  # Código armazenado no S3
  s3_bucket = "controle-armazem-code-bucket"
  s3_key    = "controle_armazem_code.zip"
}

# Se precisar que API Gateway invoque a Lambda:
resource "aws_lambda_permission" "allow_apigw" {
  statement_id  = "AllowAPIGatewayInvoke"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.controle_armazem_lambda.function_name
  principal     = "apigateway.amazonaws.com"
  # source_arn = "arn:aws:execute-api:..."  # opcionalmente restrinja ao seu API
}

resource "aws_iam_role" "lambda_execution_role" {
  name = "lambda-controleArmazem-execution-role"
  assume_role_policy = jsonencode({
    Version = "2012-10-17",
    Statement : [
      {
        Action = "sts:AssumeRole",
        Effect = "Allow",
        Principal = {
          Service = "lambda.amazonaws.com"
        }
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "lambda_policy_attachment" {
  role       = aws_iam_role.lambda_execution_role.name
  policy_arn = aws_iam_policy.lambda_policy.arn
}

//Regra para acesso da lambda ao DynamoDB
resource "aws_iam_policy" "lambda_policy" {
  name        = "lambda-controleArmazem-policy"
  description = "Policy for Lambda to access DynamoDB"
  policy = jsonencode({
    Version = "2012-10-17",
    Statement : [
      {
        Action = [
          "dynamodb:PutItem",
          "dynamodb:GetItem",
          "dynamodb:UpdateItem",
          "dynamodb:DeleteItem",
          "dynamodb:Scan",
          "dynamodb:Query"
        ],
        Effect   = "Allow",
        Resource = "*"
      },
      {
        Action = [
          "logs:CreateLogGroup",
          "logs:CreateLogStream",
          "logs:PutLogEvents"
        ],
        Effect   = "Allow",
        Resource = "*"
      }
    ]
  })
}

resource "aws_dynamodb_table" "controle_armazem_table_Produtos" {
  name         = "Produtos"
  billing_mode = "PAY_PER_REQUEST"
  hash_key     = "Id"

  attribute {
    name = "Id"
    type = "S"
  }
}

resource "aws_dynamodb_table" "controle_armazem_table_Categoria" {
  name         = "Categorias"
  billing_mode = "PAY_PER_REQUEST"
  hash_key     = "Id"

  attribute {
    name = "Id"
    type = "S"
  }
}