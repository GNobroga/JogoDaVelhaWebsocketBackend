# JogoDaVelhaWebsocketBackend

Backend para servir como ponta pé para o projeto contido no repositório -> https://github.com/GNobroga/JogoDaVelhaWebsocketFront


## Endpoints

### Account

#### /account/create-account (POST)

**Payload** 

```json
    {
        "username": "username",
        "email": "email@email.com",
        "password": "password",
        "confirmPassword": "password"
    }
```

#### /account/forgot-account (PATCH)

**Payload**

```json
    {
        "email": "email@email.com",
        "password": "password",
        "confirmPassword": "password"
    }
```

#### /account/confirm-email (POST)

**Payload**

```json
    {
        "email": "email@email.com"
    }
```

### Auth

#### /api/auth (POST)

**Payload**

```json
    {
        "email": "email@email.com",
        "password": "password",
    }
```

**Retorno**

```json
    {
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0aGFsbGVzanVAZ21haWwuY29tIiwiZXhwIjoxNzA0NzU1NzI5LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUxODQifQ.bmIbZtT-fn6xZM2BP72e6oOg02R9QXfzNaC6Sm9Y4Es"
    }
```


## Users

#### /api/users (GET)

#### /api/users (POST)

#### /api/users/{id} (GET)
 
#### /api/users/{id} (PUT)

#### /api/users/{id} (DELETE)


