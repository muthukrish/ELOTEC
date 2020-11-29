/*************************
Name		: Muthukrishnan
************************/
do $$
begin
	if exists (select * from pg_proc where proname='fn_getregisteredroomfor_device') 
	then
		drop function fn_getregisteredroomfor_device;
	end if;
end$$;

CREATE OR REPLACE FUNCTION public.fn_getregisteredroomfor_device(
	blid character varying)
    RETURNS SETOF rooms 
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
    ROWS 1000
    
AS $BODY$
BEGIN
   RETURN query select * from rooms R 
				where R.roomid = (select roomid from masterdevices MD where MD.bleid=blid);
END; $BODY$;

ALTER FUNCTION public.fn_getregisteredroomfor_device(character varying)
    OWNER TO postgres;





