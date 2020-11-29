/*************************
Name		: Muthukrishnan

************************/
do $$
begin
	if exists (select * from pg_proc where proname='sp_registerroomtodevice') 
	then
		DROP PROCEDURE public.sp_registerroomtodevice(integer, integer);
	end if;
end$$;

CREATE OR REPLACE PROCEDURE public.sp_registerroomtodevice(
	did integer,
	rid integer)
LANGUAGE 'plpgsql'

AS $BODY$
BEGIN
if rid=0 then
update masterdevices set roomid	= null where masterdeviceid	= did;
else
update masterdevices set roomid	= rid where masterdeviceid	= did;
end if;
commit;
END;
$BODY$;

