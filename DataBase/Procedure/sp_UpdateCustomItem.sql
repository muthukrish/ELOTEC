/*************************
Name		: Muthukrishnan

************************/
do $$
begin
	if exists (select * from pg_proc where proname='sp_updatecustomitem') 
	then
		DROP PROCEDURE public.sp_updatecustomitem(integer, integer, boolean);
	end if;
end$$;

CREATE OR REPLACE PROCEDURE public.sp_updatecustomitem(
	rid integer,
	riid integer,
	isactiveval boolean)
LANGUAGE 'plpgsql'

AS $BODY$
BEGIN
Update roomobjects set isactive=isactiveVal where roomid=rid and roomitemid=riid;
	
    COMMIT;
END;
$BODY$;

