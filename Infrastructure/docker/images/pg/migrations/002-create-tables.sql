CREATE TABLE Roles (
                       RoleID SERIAL PRIMARY KEY,
                       Name VARCHAR(255) NOT NULL
);

CREATE TABLE Users (
                       UserID SERIAL PRIMARY KEY,
                       Username VARCHAR(100) NOT NULL,
                       Password VARCHAR(100) NOT NULL,
                       RoleID INTEGER REFERENCES Roles(RoleID)
);

CREATE TABLE Skills (
                        SkillID SERIAL PRIMARY KEY,
                        Name VARCHAR(100) NOT NULL,
                        ProfessionalArea VARCHAR(100) NOT NULL
);

CREATE TABLE Professionals (
                               ProfessionalID SERIAL PRIMARY KEY,
                               UserID INTEGER REFERENCES Users(UserID),
                               Name VARCHAR(100) NOT NULL,
                               Country VARCHAR(100) NOT NULL,
                               Email VARCHAR(100) NOT NULL,
                               HourlyRate NUMERIC NOT NULL,
                               Visibility VARCHAR(50) NOT NULL
);

CREATE TABLE Professional_Skills (
                                     ProfessionalID INTEGER REFERENCES Professionals(ProfessionalID),
                                     SkillID INTEGER REFERENCES Skills(SkillID),
                                     YearsExperience INTEGER NOT NULL,
                                     PRIMARY KEY (ProfessionalID, SkillID)
);

CREATE TABLE Experiences (
                             ExperienceID SERIAL PRIMARY KEY,
                             ProfessionalID INTEGER REFERENCES Professionals(ProfessionalID),
                             Title VARCHAR(200) NOT NULL,
                             Company VARCHAR(100) NOT NULL,
                             StartYear INTEGER NOT NULL,
                             EndYear INTEGER
);

CREATE TABLE Clients (
                         ClientID SERIAL PRIMARY KEY,
                         UserID INTEGER REFERENCES Users(UserID),
                         Name VARCHAR(100) NOT NULL
);

CREATE TABLE JobProposals (
                              JobProposalID SERIAL PRIMARY KEY,
                              UserID INTEGER REFERENCES Users(UserID),
                              ClientID INTEGER REFERENCES Clients(ClientID),
                              Name VARCHAR(200) NOT NULL,
                              TalentCategory VARCHAR(100) NOT NULL,
                              TotalHours INTEGER NOT NULL,
                              JobDescription TEXT NOT NULL
);

CREATE TABLE JobProposal_Skills (
                                    JobProposalID INTEGER REFERENCES JobProposals(JobProposalID),
                                    SkillID INTEGER REFERENCES Skills(SkillID),
                                    MinYearsExperience INTEGER NOT NULL,
                                    PRIMARY KEY (JobProposalID, SkillID)
);