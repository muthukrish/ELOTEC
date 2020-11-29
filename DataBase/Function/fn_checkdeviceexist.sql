/*************************
Name		: Muthukrishnan
************************/
do $$
begin
	if exists (select * from pg_proc where proname='fn_checkdeviceexist') 
	then
		drop function fn_checkdeviceexist;
	end if;
end$$;

CREATE OR REPLACE FUNCTION public.fn_checkdeviceexist(
	blid character varying)
    RETURNS SETOF masterdevices 
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
    ROWS 1000
    
AS $BODY$
BEGIN
   RETURN query select * from masterdevices MD 
				where MD.bleid = blid;
END; $BODY$;

ALTER FUNCTION public.fn_checkdeviceexist(character varying)
    OWNER TO postgres;




