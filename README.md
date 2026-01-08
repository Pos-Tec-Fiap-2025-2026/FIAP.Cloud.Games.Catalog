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

## ⚙️ Como Executar o Projeto (Clone & Run)

### 🔧 Pré-requisitos

Para executar o serviço, é necessário ter instalado:

- **Docker Desktop**
- **.NET 9 SDK**

### **Executando a Solução Completa**

Este projeto possui orquestração unificada. Para subir a API e o RabbitMQ juntos, execute na raiz do projeto:

```bash
docker-compose up
```

📍 **Acesse a API:** [http://localhost:5001/swagger](http://localhost:5001/swagger)
📍 **Acesse o RabbitMQ:** [http://localhost:15672](http://localhost:15672) (usuário: `guest` / senha: `guest`)

---

## Orquestração com Kubernetes

Este microsserviço possui manifestos prontos para deploy em Kubernetes.

### Como Fazer o Deploy (Local)
1. Garanta que o Kubernetes está habilitado no Docker Desktop.
2. Construa a imagem localmente (necessário para o Kind/Minikube/Docker Desktop):
```bash
docker build -f Catalog.API/Dockerfile -t catalog-api:latest .
```
3. Na raiz do projeto, aplique os manifestos:

```bash
kubectl apply -f k8s/
```

**Verificar Status**
```bash
kubectl get pods
```

**Acessar a Aplicação**
Para acessar o Swagger e testar (fazendo o bind para a porta 5001 para evitar conflitos):
```bash
kubectl port-forward svc/catalog-service 5001:80
```
📍 **Swagger K8s:** [http://localhost:5001/swagger](http://localhost:5001/swagger)

**Remover Deploy**
Para limpar o cluster:
```bash
kubectl delete -f k8s/
```

## 📘 Observações Importantes

- O projeto **possui Swagger**, acessível em `/swagger`.

- A persistência dos dados é feita **em memória** para fins de demonstração (os dados resetam ao reiniciar o pod ou container).

- O deploy no Kubernetes está configurado com **1 réplica** para garantir consistência dos dados em memória.

- O **MassTransit** gerencia a conexão e publicação de eventos no RabbitMQ automaticamente.