# CurrencyConverter


Este repositório contém **duas versões distintas** de um conversor de moedas desenvolvido com **.NET (WPF)** como front-end. Ambas implementam a lógica de conversão, mas com arquiteturas diferentes:

- 🔗 **Versão 1 – `api-version`**: consumo de uma API externa de câmbio em tempo real.
- 🗄 **Versão 2 – `db-version`**: uso de um banco de dados local com taxas armazenadas e modificaveis pelo usuario.

---

## 🧩 Tecnologias Utilizadas

- ✔️ **.NET Framework / WPF**
- ✔️ **C#**
- ✔️ **XAML (interface WPF)**
- ✔️ **Entity Framework (na versão com banco de dados)**
- ✔️ **HTTPClient (na versão com API)**
- ✔️ **JSON Serialization (para leitura das taxas)**


---

## 📂 Estrutura de Branches

| Branch         | Descrição                                                                |
|----------------|--------------------------------------------------------------------------|
| `api-version`  | Consome uma API externa para obter taxas de câmbio em tempo real.        |
| `db-version`   | Utiliza um banco de dados local com taxas pré-definidas.                 |

---

## 🔗 `api-version` – Conversor com API

### 🌐 Funcionalidade
- Realiza chamadas HTTP a uma API externa de câmbio.
- Atualiza os valores em tempo real conforme a resposta da API.
- Interface limpa e responsiva construída com WPF.

### ⚙️ Tecnologias extras
- `HttpClient`
- `System.Text.Json` ou `Newtonsoft.Json` (para parsing JSON)
- API pública (como [ExchangeRate API](https://www.exchangerate-api.com/) ou similar)

---

## 🗄 `db-version` – Conversor com Banco de Dados

### 🧱 Funcionalidade
- Armazena as taxas de câmbio em um banco de dados local (SQLite ou SQL Server LocalDB).
- Permite CRUD das moedas e taxas.
- Interface WPF similar à da versão API.

### ⚙️ Tecnologias extras
- `Entity Framework`
- `DbContext`, `Migrations`
- Banco local (MicrosoftSQLServer)

---

## 📦 Como Usar

### 📥 Clonar o projeto:

```bash
git clone https://github.com/FrancescoTalento/CurrencyConverter.git
cd CurrencyConverter
