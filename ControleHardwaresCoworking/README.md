# Controle de Hardwares do Coworking

Sistema de gestão de inventário para hardwares e equipamentos do coworking. Controla entrada, saída e movimentação de produtos com rastreamento completo de colaboradores responsáveis.

## 📋 Sobre o Projeto

Ferramenta interna para gerenciamento de patrimônio e estoque de hardwares do coworking. O sistema centraliza:

- Cadastro de produtos e equipamentos
- Entrada e saída de estoque
- Ajuste de estoque
- Atribuição de hardwares a colaboradores
- Manutenção de colaboradores
- Relatórios de movimentação

**Status:** ✅ Em produção

## 🛠 Stack Tecnológico

- **Linguagem:** C#
- **Framework:** .NET Framework
- **Padrão:** Repository Pattern + Services
- **Banco de Dados:** SQL Server

## ✨ Funcionalidades Principais

- ✅ Cadastro de produtos (nome, descrição, valor)
- ✅ Gestão de colaboradores (atribuição de hardwares)
- ✅ Entrada de estoque (recebimento de equipamentos)
- ✅ Saída de estoque (entrega a colaboradores)
- ✅ Ajuste de estoque (correção de quantidades)
- ✅ Manutenção de colaboradores (atualização de dados)
- ✅ Movimentações completas com histórico
- ✅ Relatórios detalhados de movimentação

## 📁 Estrutura do Projeto

```
ControleHardwaresCoworking/
├── BancoDados/
│   └── ConexaoBD.cs              # Gerenciamento de conexão SQL Server
├── Entities/
│   ├── Core/
│   │   ├── Colaborador.cs        # Entidade de colaborador
│   │   ├── Movimentacao.cs       # Histórico de movimentações
│   │   └── Produto.cs            # Entidade de produto/equipamento
│   ├── Dtos/
│   │   ├── MovimentacaoRelatorio.cs  # DTO para relatórios
│   │   ├── Entity.cs             # Classe base
│   │   └── Utils.cs              # Utilitários
│   └── Interfaces/
│       └── IServices.cs          # Interface de serviços
├── Repositories/
│   ├── ColaboradorRepository.cs  # Acesso a dados de colaboradores
│   ├── EstoqueRepository.cs      # Acesso a dados de estoque
│   └── MovimentacaoRepository.cs # Acesso a movimentações
├── Services/
│   ├── AjusteEstoqueService.cs   # Ajuste de quantidades
│   ├── CadastrarItemService.cs   # Cadastro de produtos
│   ├── EntradaEstoqueService.cs  # Entrada de equipamentos
│   ├── ManutencaoColaboradorService.cs # Manutenção de colaboradores
│   ├── MovimentacoesService.cs   # Gestão de movimentações
│   └── SaidaEstoqueService.cs    # Saída de equipamentos
├── Properties/
│   └── AssemblyInfo.cs           # Informações do assembly
├── App.config                    # Configuração da aplicação
├── Program.cs                    # Ponto de entrada
├── FodyWeavers.xml              # Configuração do Fody
└── packages.config              # Dependências NuGet
```

## 🚀 Como Executar

### Pré-requisitos
- .NET Framework 4.6.1 ou superior
- SQL Server 2016+
- Visual Studio 2015 ou superior

### Instalação

1. Clone o repositório:
```bash
git clone https://github.com/lucasody96/ControleHardwaresCoworking.git
cd ControleHardwaresCoworking
```

2. Atualize a connection string em `App.config`:
```xml
<configuration>
  <connectionStrings>
    <add name="DefaultConnection" 
         connectionString="Server=SEU_SERVER;Database=ControleHardwaresCoworking;User Id=sa;Password=SUA_SENHA;" 
         providerName="System.Data.SqlClient" />
  </connectionStrings>
</configuration>
```

3. Restaure os pacotes NuGet:
```bash
nuget restore
```

4. Compile a solução:
```bash
msbuild ControleHardwaresCoworking.sln
```

5. Execute a aplicação:
```bash
ControleHardwaresCoworking.exe
```

## 📖 Como Usar

### 📦 Cadastrar Produto
1. Acesse o menu de **Cadastro de Items**
2. Informe o nome do equipamento
3. Descrição 
4. Valor do equipamento
5. Confirmar cadastro

### 📥 Entrada de Estoque
1. Vá para **Entrada de Estoque**
2. Selecione o produto
3. Informe a quantidade recebida
4. Data de entrada
5. Registrar entrada

### 📤 Saída de Estoque
1. Acesse **Saída de Estoque**
2. Selecione o produto
3. Escolha o colaborador responsável
4. Quantidade a entregar
5. Registrar saída

### 🔧 Manutenção de Colaborador
1. Menu **Manutenção de Colaborador**
2. Busque o colaborador
3. Atualize informações
4. Salvar alterações

### ⚙️ Ajuste de Estoque
1. **Ajuste de Estoque**
2. Selecione o produto
3. Informe o ajuste (aumento ou redução)
4. Motivo do ajuste
5. Confirmar

### 📊 Relatório de Movimentações
1. Acesse **Movimentações**
2. Filtre por período, produto ou colaborador
3. Visualize histórico completo
4. Exporte relatório (se aplicável)

## 💡 Fluxo Principal

1. **Cadastro** → Registra produto no banco
2. **Entrada** → Recebe equipamento, aumenta quantidade
3. **Saída** → Entrega a colaborador, reduz quantidade
4. **Movimentação** → Sistema registra todas as alterações
5. **Relatório** → Consulta histórico completo

## 📧 Contato

**Desenvolvedor:** Lucas Ody  
**Email:** lucasody@gmail.com  
**Posição:** Support Leader @ Linx

## 📄 Licença

Proprietary - Linx Informática

---

*Última atualização: Maio 2026*  
*Status: ✅ Em Produção*
