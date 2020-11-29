/*************************
Name		: Muthukrishnan
************************/
do $$
begin
	if exists (select * from pg_proc where proname='fn_CheckLogin') 
	then
		drop function fn_CheckLogin;
	end if;
end$$;
CREATE OR REPLACE FUNCTION public.fn_checklogin(
	uname character varying DEFAULT NULL::character varying,
	passw character varying DEFAULT NULL::character varying,
	OUT useridval integer)
    RETURNS integer
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
    
AS $BODY$
BEGIN
UserIdVal := (select userid from users where username = uname and password=passw);

END
$BODY$;

ALTER FUNCTION public.fn_checklogin(character varying, character varying)
    OWNER TO postgres;
