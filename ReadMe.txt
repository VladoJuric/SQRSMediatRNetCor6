Projekt je napravljen u .Net Core 6.0 clean architecture Mediator SQLRS pattern s AngularScaffold-om za automatsko generiranje frontend API servisa i modela. Za pokretanje scaffolda potrebno instalirati java jdk i dodati Path u Environment Varijable.
Postoje 2 FrontEnd projekta u Angularu na lokaciji \FrontEnds\ (BackOffice, PrometSPA)

Korišten je Dependency Injection kao i Fluent Validation za validaciju svih Dto-a s fronta, SignalR, Redis i RestSharp.

 
Backend projekt se sastoji od 4 layera:

- WebUI - Api Controllers, OpenApi generator, Exception Filteri

- Infrastructure - Logger, ApplicationDBContext (SQLServer), Redis Caching, Entity Configuracije, Migracije (CodeFirst), i potencijalno EmailService

- Domain - Entiteti, Enumi, Audit, PredicateBuilder za gradenje dinamiskih query-a

- Application -  Commands, Queries, Dtos sortirani po Controllerima (Administrator, FrontDesk, User), Mapiranje Entiteta i Dto-a, FluentValidation dodan za sve Dto-e sa fronta, dodan hub za signalR i notifikacije

