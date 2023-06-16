insert into Roles (Name) values ('admin');
insert into Roles (Name) values ('user-manager');
insert into Roles (Name) values ('user');

insert into Users (Username, Password, RoleID) values ('root','root', (Select RoleID from Roles WHERE Name = 'admin' LIMIT 1));