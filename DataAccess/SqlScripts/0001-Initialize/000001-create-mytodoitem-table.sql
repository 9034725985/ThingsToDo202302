﻿--drop table if exists mytodoitem;

--create table mytodoitem
--(
--	id int primary key generated by default as identity, 
--	externalid uuid not null unique default gen_random_uuid (),
--	title varchar(2000) null,
--	description varchar(4000) not null,
--	isdone boolean not null default false,
--	createdby int not null default 0,
--	createddate timestamp with time zone not null default now(),
--	modifiedby int not null default 0,
--	modifieddate timestamp with time zone not null default now()
--);