/*************************
Name		: Muthukrishnan
************************/
do $$
begin
	if exists (select * from pg_proc where proname='fn_GetDeviceInformation') 
	then
		drop function fn_GetDeviceInformation;
	end if;
end$$;

CREATE OR REPLACE FUNCTION public.fn_getdeviceinformation(
	uid integer,
	did integer)
    RETURNS TABLE(deviceid smallint, devicename character varying, userid integer, roomnoid smallint, itemid smallint, item character varying, axis character varying, isregistered integer, updated_date timestamp without time zone, lastupdateduser character varying, isactive boolean) 
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
    ROWS 1000
    
AS $BODY$
BEGIN
   RETURN query
  SELECT masterdeviceid as DeviceId 
	,(  
     SELECT name  
     FROM masterdevices DD  
     WHERE DD.masterdeviceid = RD.masterdeviceid  
     ) AS DeviceName   
   ,modifiedby as UserId  
    ,roomid as RoomNoId  
    ,roomitemid as ItemId 
	 ,(  
     SELECT name  
     FROM roomitems ItmD  
     WHERE ItmD.roomitemid = RD.roomitemid  
     ) AS Item     
    ,location as Axis
	,(case when location IS NULL  then 0 else 1 end) AS IsRegistered
	, modifiedon AS Updated_Date	  
    ,(  
     SELECT FirstName  
     FROM Users U  
     WHERE U.UserId = RD.modifiedby  
     ) AS LastUpdatedUser   
    ,RD.IsActive  
    FROM roomobjects RD  
   WHERE masterdeviceid = did 
    AND RD.IsActive = true;  
END;
$BODY$;

ALTER FUNCTION public.fn_getdeviceinformation(integer, integer)
    OWNER TO postgres;


