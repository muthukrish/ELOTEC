/*************************
Name		: Muthukrishnan
************************/
do $$
begin
	if exists (select * from pg_proc where proname='fn_getunregisteredroomlist') 
	then
		drop function fn_getunregisteredroomlist;
	end if;
end$$;

CREATE OR REPLACE FUNCTION public.fn_getunregisteredroomlist(
	)
    RETURNS SETOF rooms 
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
    ROWS 1000
    
AS $BODY$
BEGIN
   RETURN query select * from rooms 
				where roomid not in (select roomid from masterdevices where roomid is not null);
END; $BODY$;

ALTER FUNCTION public.fn_getunregisteredroomlist()
    OWNER TO postgres;






