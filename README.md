# PAF

No âmbito da discipilina curricular Sistemas Gráficos e Interação (2017-2018), foi desenvolvida uma proposta de interface, através do desenvolvimento de um Protótipo de Alta Fidelidade (PAF) funcional, para o site do curso de Engenharia Informática da Escola Superior de Tecnologia e Gestão do Instituto Politécnico de Leiria (ESTG-IPLeiria). 
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
