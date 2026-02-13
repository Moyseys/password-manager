# Password Manager 🔐

Gerenciador de senhas com **arquitetura zero-knowledge**: toda derivação de chave e criptografia ocorre no cliente, e o backend recebe apenas dados cifrados. Assim, o servidor não consegue descriptografar segredos nem reconstruir chaves.

Autenticação via **JWT** armazenado em **cookie HttpOnly**, reduzindo exposição a scripts e mantendo sessões seguras.

Backend organizado em **microserviços**, com gateway para roteamento e isolamento de responsabilidades.
