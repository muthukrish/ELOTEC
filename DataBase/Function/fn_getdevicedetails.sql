/*************************
Name		: Muthukrishnan
************************/
do $$
begin
	if exists (select * from pg_proc where proname='fn_GetDeviceDetails') 
	then
		drop function fn_GetDeviceDetails;
	end if;
end$$;

-- FUNCTION: public.fn_getdevicedetails(integer, integer)

-- DROP FUNCTION public.fn_getdevicedetails(integer, integer);

CREATE OR REPLACE FUNCTION public.fn_getdevicedetails(
	uid integer,
	did integer)
    RETURNS TABLE(deviceid smallint, device character varying, userid integer, roomnoid smallint, itemid smallint, item character varying, axis character varying, isregistered integer, isstatic boolean) 
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
    ROWS 1000
    
AS $BODY$
BEGIN
  INSERT INTO roomobjects (roomid ,roomitemid,masterdeviceid,location,isactive,modifiedby,modifiedon ) 
  select 1,roomitemid,did ,'',true,uid,now()  from roomitems where roomitemid not in(select roomitemid from roomobjects RObJ where RObJ.masterdeviceid=did);
	   
   RETURN query (SELECT masterdeviceid as DeviceId  
    ,(  
     SELECT name  
     FROM masterdevices DD  
     WHERE DD.masterdeviceid = RD.masterdeviceid  
     ) AS Device  
    ,modifiedby as UserId  
    ,roomid as RoomNoId  
    ,roomitemid as ItemId  
    ,(  
     SELECT name  
     FROM roomitems ItmD  
     WHERE ItmD.roomitemid = RD.roomitemid  
     ) AS Item  
    ,location As Axis 
    ,(case when location IS NULL OR location = '' then 0 else 1 end) AS IsRegistered    
	,(  
    SELECT ItmD.isstatic  
    FROM roomitems ItmD  
    WHERE ItmD.roomitemid = RD.roomitemid  
    ) AS isstatic 
   FROM roomobjects RD  
   WHERE masterdeviceid = did   
    AND RD.IsActive = true   order by ItemId asc);
END; $BODY$;

ALTER FUNCTION public.fn_getdevicedetails(integer, integer)
    OWNER TO postgres;



