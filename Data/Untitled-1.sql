

-- select * from Clients
-- where ClientId='map'
update Clients
-- set ClientUri='https://localhost:5003'
set RequireConsent="0"
where ClientId='map'

-- update AspNetusers
-- set SubjectId="2"
-- where UserName="bob"