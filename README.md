# RV Test

Primeiro precisamos gerar a imagem docker localmente, rode docker build -t rv-test -f RV.Test/Dockerfile . 
a partir da raiz do projeto

Depois de gerada a imagem, aloque no mínimo 4gb no Docker-Engine, pois usaremos o SQL Server on Linux e ele é bem pesado

