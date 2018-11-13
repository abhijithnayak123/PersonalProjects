delete from tChxr_Identity
delete from tChxr_Session

select rowguid, Id, Name, URL, DTCreate, DTLastMod into #tmpPartner from tChxr_Partner

delete from tChxr_Partner

alter table tChxr_Partner drop column Id
alter table tChxr_Partner add Id bigint not null

insert tChxr_Partner(rowguid, Id, Name, URL, DTCreate, DTLastMod)
select rowguid, Id, Name, URL, DTCreate, DTLastMod from #tmpPartner

drop table #tmpPartner