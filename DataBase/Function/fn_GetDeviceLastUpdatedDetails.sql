/*************************
Name		: Muthukrishnan
************************/
do $$
begin
	if exists (select * from pg_proc where proname='fn_GetDeviceLastUpdatedDetails') 
	then
		drop function fn_GetDeviceLastUpdatedDetails;
	end if;
end$$;
CREATE OR REPLACE FUNCTION public.fn_getdevicelastupdateddetails(
	did integer)
    RETURNS TABLE(updated_date timestamp without time zone, lastupdateduser character varying, deviceid smallint, devicename character varying) 
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
    ROWS 1000
    
AS $BODY$
BEGIN
   RETURN query SELECT modifiedon AS Updated_Date 
 ,(  
  SELECT FirstName  
  FROM Users U  
  WHERE UserId = RD.modifiedby and IsActive=true  
  ) AS LastUpdatedUser  
  ,masterdeviceid AS DeviceId  
  ,(SELECT name FROM masterdevices DD WHERE DD.masterdeviceid = RD.masterdeviceid) AS DeviceName  
FROM roomobjects RD  
WHERE masterdeviceid = did and IsActive=true  
ORDER BY Updated_Date DESC LIMIT 1 ;
END;
$BODY$;

ALTER FUNCTION public.fn_getdevicelastupdateddetails(integer)
    OWNER TO postgres;

