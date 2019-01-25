-- delete from PersistedGrants;
-- delete from IdentityResources;
-- delete from IdentityClaims;
--     delete from ClientScopes


-- delete from AspNetUserClaims
-- where Id=18


insert into AspNetRoleClaims(RoleId,claimType,ClaimValue)
values(1, "AO Encoder","AO Dashboard")