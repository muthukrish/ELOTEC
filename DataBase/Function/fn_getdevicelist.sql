/*************************
Name		: Muthukrishnan
************************/
do $$
begin
	if exists (select * from pg_proc where proname='fn_getdevicelist') 
	then
		drop function fn_getdevicelist;
	end if;
end$$;

CREATE OR REPLACE FUNCTION public.fn_getdevicelist(
	filterstr text,
	page integer,
	size integer)
    RETURNS TABLE(deviceid smallint, devicename character varying, isactive boolean, updated_date timestamp without time zone, userid integer, lastupdateduser character varying, registeredstatus integer) 
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
    ROWS 1000
    
AS $BODY$
declare qstring text;  
declare offsetVal INT := (page - 1) * size;  
BEGIN
	qstring := 'select DeviceId,DeviceName,IsActive,Updated_Date,UserId,LastUpdatedUser,RegisteredStatus from (  
    SELECT masterdeviceid As DeviceId 
    ,name As DeviceName  
   ,IsActive  
   ,(select max(modifiedon) from roomobjects RO where RO.masterdeviceid=DD.masterdeviceid) As Updated_Date  
   ,(select modifiedby from roomobjects rob
where (
  modifiedon = (
    select max(modifiedon)
    from roomobjects robj
    where robj.masterdeviceid = DD.masterdeviceid
  )
  and rob.masterdeviceid = DD.masterdeviceid
  and ISACTIVE = true
)
limit 1) AS UserId  
   ,(  
   SELECT FirstName  
   FROM Users U  
   WHERE UserId = (select modifiedby from roomobjects rob
where (
  modifiedon = (
    select max(modifiedon)
    from roomobjects roo
    where roo.masterdeviceid = DD.masterdeviceid
  )
  and rob.masterdeviceid = DD.masterdeviceid
  and ISACTIVE = true
)
limit 1)  
   ) AS LastUpdatedUser  
   ,CASE when (select count(*) from roomobjects rrr where (rrr.location is null and rrr.IsActive=true and rrr.masterdeviceid=DD.masterdeviceid) ) > 0  
   THEN 0  
   ELSE 1  
   END AS RegisteredStatus  
   ,ROW_NUMBER() OVER (order by DD.masterdeviceid asc) AS RowNum  
  from masterdevices DD  
  ) AS x ' ;
  

qstring := qstring || ' ' || filterStr;
qstring := qstring || ' ' || 'and RowNum BETWEEN';
qstring := qstring || ' ' || offsetVal ;
qstring := qstring || ' ' || 'AND';
qstring := qstring || ' ' || (offsetVal+size) ;

return query execute qstring;
END; $BODY$;

ALTER FUNCTION public.fn_getdevicelist(text, integer, integer)
    OWNER TO postgres;



