# FIAP Cloud Games – Catalog

Microsserviço responsável pelo gerenciamento do catálogo de jogos (CRUD) e publicação de eventos de integração para a plataforma FIAP Cloud Games.

---

## 🚀 Tecnologias Utilizadas

- **.NET 9** (Web API)
- **MassTransit**
- **RabbitMQ**
- **Docker & Docker Compose**
- **Kubernetes** (Manifestos de Orquestração)
- **Clean Architecture**
- **Swagger** (Documentação da API)

---

## ⚙️ Como Executar o Projeto com Docker compose

### 🔧 Pré-requisitos

Para executar o serviço, é necessário ter instalado:

- **Docker Desktop**
- **.NET 9 SDK**

---

### **1. Configuração (AppSettings)**

Para que a conexão com o RabbitMQ funcione localmente, verifique se o seu arquivo `appsettings.Development.json` possui a seção de configuração do barramento:

```json
"bus": {
  "user": "guest",
  "password": "guest",
  "host": "localhost",
  "port": 5672
}
```

### **2. Subir a Infraestrutura (RabbitMQ)**

Abra o terminal e navegue até a pasta onde está o arquivo docker-compose.yml (na raiz):

```bash
docker-compose up -d
```
📍 **Acesse o painel:** [http://localhost:15672](http://localhost:15672)
* **Usuário:** `guest`
* **Senha:** `guest`


### **3. Iniciar o Microsserviço**

Execute:

```bash
dotnet run --project Catalog.API
```

Ao iniciar, deve aparecer no console:

```
Bus started: rabbitmq://localhost/
```

Isso confirma que a API está conectada ao RabbitMQ e pronta para publicar eventos.

---

## Orquestração com Kubernetes

Este microsserviço possui manifestos prontos para deploy em Kubernetes.

### Como Fazer o Deploy (Local)
1. Garanta que o Kubernetes está habilitado no Docker Desktop.
2. Como a imagem é local, construa ela antes de aplicar os manifestos:
```bash
docker build -f Catalog.API/Dockerfile -t catalog-api:latest .
```
3. Na raiz do projeto, execute:

```bash
kubectl apply -f k8s/
```

**Verificar Status**
Para ver se os pods subiram:
```bash
kubectl get pods
```

**Acessar a Aplicação**
Para acessar o Swagger e testar (necessário liberar porta):
```bash
kubectl port-forward svc/catalog-service 5000:80
```
📍 **Swagger:** [http://localhost:5000/swagger](http://localhost:5000/swagger)

**Remover Deploy**
Para limpar o cluster:
```bash
kubectl delete -f k8s/
```

## 📘 Observações Importantes

- O projeto **possui Swagger**, acessível em `/swagger`.

- A persistência dos dados é feita **em memória** para fins de demonstração (os dados resetam ao reiniciar o pod).

- O deploy no Kubernetes está configurado com **1 réplica** para garantir consistência dos dados em memória.

- O **MassTransit** gerencia a conexão e publicação de eventos no RabbitMQ automaticamente.