/*************************
Name		: Muthukrishnan
************************/
do $$
begin
	if exists (select * from pg_proc where proname='fn_checkroomregisteredornot') 
	then
		drop function fn_checkroomregisteredornot;
	end if;
end$$;

CREATE OR REPLACE FUNCTION public.fn_checkroomregisteredornot(
	rid integer,
	did integer)
    RETURNS SETOF masterdevices 
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
    ROWS 1000
    
AS $BODY$
BEGIN
   RETURN query select * from masterdevices MD 
				where MD.roomid = rid and MD.masterdeviceid !=did;
END; $BODY$;

ALTER FUNCTION public.fn_checkroomregisteredornot(integer, integer)
    OWNER TO postgres;






