--Ctrl + 3
select top 100 ShortCut='Top 100',* from 

--Ctrl + 4 : Mostrar el count con comas en los miles
select Count=left(convert(varchar,convert(money, count(*)),1),charindex('.', convert(varchar,convert(money, count(1)),1))-1) from  

--Ctrl + 5 : Buscar en el nombre de los objetos
SELECT DISTINCT o.name AS Object_Name,o.type_desc FROM sys.schemas s INNER JOIN sys.objects o ON s.schema_id = o.schema_id WHERE o.name Like 

--Ctrl + 6 : Buscar dentro del codigo de los objetos
SELECT DISTINCT o.name AS Object_Name,o.type_desc FROM sys.sql_modules m INNER JOIN sys.objects o ON m.object_id = o.object_id WHERE m.definition Like 

--Ctrl + 7 : Muestra el codigo del objeto seleccionado
exec sp_helptext 

--Ctrl + 8 : Busca una columna y muestra la tabla y el esquema a la que pertenece
with cte([Schema],[Table],[Column],[Type]) as (select s.name, t.name, c.name, y.name from sys.columns c join sys.tables t on c.object_id = t.object_id join sys.schemas s on t.schema_id = s.schema_id join sys.types y on c.system_type_id = y.system_type_id) select * from cte where [Column] like 
