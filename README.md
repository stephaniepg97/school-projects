# PAF

No âmbito da disciplina curricular Sistemas Gráficos e Interação (2017-2018), foi desenvolvida uma proposta de interface, através do desenvolvimento de um Protótipo de Alta Fidelidade (PAF) funcional, para o site do curso de Engenharia Informática da Escola Superior de Tecnologia e Gestão do Instituto Politécnico de Leiria (ESTG-IPLeiria). 
Para a implementação do PAF, foi utilizada a ferramenta Blender para recorrer a técnicas de modelação e animação 3D e capacitar interatividade necessária para simular as funcionalidades do protótipo.

<b>Tecnologias utilizadas: _Blender_</b>

# CampusPark

No âmbito da disciplina curricular Integração de Sistemas (2018-2019), foi desenvolvida uma solução CampusPark para controlo da disponibidade de lugares de estacionamento, em tempo real, na Escola Superior de Tecnologia e Gestão do Instituto Politécnico de Leiria (ESTG-IPLeiria).
A solução CampusPark implementada reúne um conjunto de micro aplicações que desempenham diferentes funcionalidades:

_BOT-SpotSensors (WCF Service Application)_

Serviço Web SOAP que cria um ficheiro com formato XML que inclui dados específicos de um determinado lugar de estacionamento (Parking Spot). Implementa a interface de comunicação entre um sensor de um Parking Spot com a aplicação ParkDACE.

_ParkDACE (Console Application)_

Recebe toda a informação fornecida pelos diversos sensores espalhados pelo Campus da ESTG.  A informação recebida seria por vários serviços Web SOAP na vida real. Para simular tal comportamento, foi adicionada uma DLL, ParkingSensorNodeDLL, que envia múltiplos dados aleatórios relativos a um determinado Parking Spot. Além disso, adiciona informação da localização (em coordenadas) do sensor que emite a informação e a reúne num ficheiro XML, ParkingNodesConfig.xml. Todo o conteúdo deste ficheiro será encaminhado para a aplicação ParkTU;

_ParkTU (Console Application)_

Plataforma de comunicação entre a fonte de dados dos sensores (ParkDACE) e outras aplicações cliente que pretendem ter acesso aos dados. Aproveita a informação fornecida no ficheiro ParkingNodesConfig.xml e preenche o ficheiro parks.xml com informação relevante dos Parks e ParkingSpots existentes no Campus. Disponibiliza o seu conteúdo em domínio público;

_ParkSS (Console Application)_

Linha de consola que automatiza a receção de dados fornecidos pelo ParkTU (o conteúdo XML do ficheiro parks.xml) e os armazena numa database relacional (ParkDB);

_SmartPark (ASP.NET Web Application)_

_RESTful Web Service_ disponível a dispositivos móveis e utilizadores finais.

_ParkDashboard (Windows Application)_

Ambiente gráfico do tipo Windows Form que monitoriza e configura Parks e Parking Spots existentes em toda a plataforma.

<b>Tecnologias utilizadas: _.NET Framework; Visual Studio IDE; Open Source Message Broker Eclipse Mosquitto™ [mosquitto.org] que implementa o protocolo MQTT [mqtt.org/]_</b>

# PFA (Personal Finances Assistant)

No âmbito da disciplina curricular Aplicações para a Internet (2017-2018), foi desenvolvida uma Web Application (ou Platform) com o objetivo principal de ajudar os utilizadores a organizar as suas contas bancárias. A aplicação permite a criação de perfis públicos de utilizadores, com permissões de acesso a informação pessoal e privada das suas contas, histórico de movimentos realizados e respetiva análise de dados dos movimentos em tabelas e gráficos.

<b>Tecnologias utilizadas: _Laravel Framework; Bootstrap Framework_</b>

# RestaurantManagement

No âmbito da disciplina curricular Desenvolvimento de Aplicações Distribuídas (2018-2019), foi implementada uma _Single Page Application_ (SPA) de gestão de pedidos num determinado restaurante, constítuida por uma aplicação cliente WEB desenvolvida com base na _framework Vue.js_ e uma aplicação servidor WEB do tipo RESTful API implementada com uso da _framework Laravel_ e de um _Web Socket Server_ em ambiente de execução _Node.js_.

<b>Tecnologias utilizadas: _Frameworks Laravel, Node.js e Vue.js_</b>

# SportsClub

No âmbito da disciplina curricular Desenvolvimento de Aplicações Empresariais (2019-2020), foi desenvolvida uma aplicação empresarial destinada a ajudar as direções dos clubes desportivos em tarefas administrativas para gerir e atualizar registos importantes através de uma plataforma digital. 
No processo de implementação da aplicação foi utilizada as tecnologias da plataforma Java Enterprise Edition, para os componentes das camadas de lógica de negócio e acesso a dados, e a plataforma Vue.js. para a camada de lógica de apresentação.

<b>Tecnologias utilizadas: _Java Enterprise Edition (JEE) e Vue.js_</b>