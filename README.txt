Banco utilizado SqlServer.


[ 0 ]

 NA PASTA PRINTS DA APLICA��O existem alguns prints de como o projeto ficou.


[ 1 ]

*** Executar os Scrips no arquivo "Scripts"

Criar a base de dados no SQlServer.

Criar um usu�rio,  e o no final do script executar os grants para o usu�rio criado;


[ 2 ] Substituir no web config do projeto Web Api a connection String correspondente ao banco SQLServer.

*** Informar a fonte de dados, 
    Informar o usu�rio e senha.

<connectionStrings>
      <add name="Conexao" connectionString="Data Source=FONTE PARA SE CONECTAR;Initial Catalog=dbEncurtadorUrl;Persist Security Info=True;User ID=Usuario;Password=Senha do Banco" providerName="System.Data.SqlClient" />
  </connectionStrings>


[ 3 ] Executar ambos os projetos para que sejam executados juntos, tanto Web (SiteEnkurtUrl) quanto WebApi.
      Bot�o direito sobre a solu��o -> Properties -> StartUp Project - Habilitar Start para os dois projetos caso n�o esteja j� marcado.

[ 3 ] Alterar no web.config do Projeto Web (SiteEnkurtUrl) dentro de AppSetings

<add key="UrlWebApi" value="http://localhost:49677/api/Stats/getStats"/>

***  SUBSTITUIR PELO CAMINHO DA WEB API PARA QUE A APLICA��O WEB POSSA CONSUMI-LA.

[ 4 ] Chamadas realizadas em local Host via PostMan, todas Funcionando.


********* SUBSTITUIR NAS CHAMADAS OS IDS CORRESPONDENTES, IDS ABAIXO APENAS EXEMPLO ********* 


[ *** Inserir Usu�rio ] 
http://localhost:49677/api/Users/Put?id=danilo

[  *** EXECUTAR VIA BROWSER PARA O REDIRECIONAMENTO DA P�GINA ACONTECER ] 
http://localhost:49677/api/Urls/GetById?id=12

[ *** Obter Status do Sistema ]
http://localhost:49677/api/Stats/getStats

*** Obter Status do Sistema por Id
http://localhost:49677/api/Stats/getStats?id=12

*** Adicionar uma Url
http://localhost:49677/api/Users/Addurl?userid=danilo&url=HTTP://www.indebug.com.br

*** Obter Status por Usu�rio
http://localhost:49677/api/Users/getStatsByUser?id=danilo

*** Deletar um usu�rio
http://localhost:49677/api/Users/Delete?id=9

*** Deletar uma Url
http://localhost:49677/api/Urls/Delete?id=9





