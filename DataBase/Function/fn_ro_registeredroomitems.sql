/*************************
Name		: Muthukrishnan
************************/
do $$
begin
	if exists (select * from pg_proc where proname='fn_ro_registeredroomitems') 
	then
		drop function fn_ro_registeredroomitems;
	end if;
end$$;

CREATE OR REPLACE FUNCTION public.fn_ro_registeredroomitems(
	rid integer)
    RETURNS TABLE(roid smallint, riid smallint, img bytea, riname character varying, loc character varying, activeobj boolean) 
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
    ROWS 1000
    
AS $BODY$BEGIN
   RETURN query select 
					ro.roomobjectid	as roid, 
					ro.roomitemid	as riid,
					ri.image		as img,
					ri.name			as riname,
					ro.location		as loc,
					ro.isactive		as activeObj
				from roomobjects ro 
				inner join roomitems ri on ri.roomitemid = ro.roomitemid
				where roomid = rid 
				order by roomobjectid;
END; $BODY$;

ALTER FUNCTION public.fn_ro_registeredroomitems(integer)
    OWNER TO postgres;
