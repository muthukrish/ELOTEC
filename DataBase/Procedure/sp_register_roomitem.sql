/*************************
Name		: Muthukrishnan

************************/
do $$
begin
	if exists (select * from pg_proc where proname='sp_register_roomitem') 
	then
		DROP PROCEDURE public.sp_register_roomitem(integer, integer, character varying, boolean, integer);
	end if;
end$$;

CREATE OR REPLACE PROCEDURE public.sp_register_roomitem(
	rid integer,
	riid integer,
	locationval character varying,
	isactiveval boolean,
	did integer)
LANGUAGE 'plpgsql'

AS $BODY$
BEGIN
	IF EXISTS (SELECT roomobjectid FROM roomobjects WHERE roomid = rid AND roomitemid = riid) THEN 
			Update roomobjects set location=locationVal,isactive=isactiveVal where roomid=rid and roomitemid=riid;
   	ELSE
		insert into roomobjects(roomid,roomitemid,masterdeviceid,location,isactive)
		values(rid,riid,did,locationVal,isactiveVal);
	END IF;
	
    COMMIT;
END;
$BODY$;
