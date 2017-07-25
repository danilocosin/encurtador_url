Banco utilizado SqlServer.

[ 1 ]

*** Executar os Scrips no arquivo "Scripts"

Criar a base de dados no SQlServer.

Criar um usuário,  e o no final do script executar os grants para o usuário criado;


[ 2 ] Substituir no web config do projeto Web Api a connection String correspondente ao banco SQLServer.

*** Informar a fonte de dados, 
    Informar o usuário e senha.

<connectionStrings>
      <add name="Conexao" connectionString="Data Source=FONTE PARA SE CONECTAR;Initial Catalog=dbEncurtadorUrl;Persist Security Info=True;User ID=Usuario;Password=Senha do Banco" providerName="System.Data.SqlClient" />
  </connectionStrings>


[ 3 ] Executar ambos os para que sejam executados juntos, tanto Web quanto WebApi.
      Botão direito sobre a solução -> Properties -> StartUp Project - Habilitar Start para os dois projetos caso não esteja já marcado.



