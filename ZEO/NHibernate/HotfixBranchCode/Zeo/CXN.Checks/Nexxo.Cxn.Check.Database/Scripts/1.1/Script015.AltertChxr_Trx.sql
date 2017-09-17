-- update table structure

select rowguid, ChexarType into #tmpTrx from tChxr_trx

alter table tChxr_Trx drop column Type
alter table tChxr_Trx drop column ChexarType
alter table tChxr_Trx add SubmitType int null
alter table tChxr_Trx add ReturnType int null

update tChxr_Trx set SubmitType = t.ChexarType, ReturnType = ChexarType
from tChxr_Trx x
join #tmpTrx t on x.rowguid = t.rowguid

drop table #tmpTrx

GO
-- update audit table structure

select rowguid, ChexarType into #tmpTrx from tChxr_trx_AUD

alter table tChxr_trx_AUD drop column Type
alter table tChxr_trx_AUD drop column ChexarType
alter table tChxr_trx_AUD add SubmitType int null
alter table tChxr_trx_AUD add ReturnType int null

update tChxr_trx_AUD set SubmitType = t.ChexarType, ReturnType = ChexarType
from tChxr_trx_AUD x
join #tmpTrx t on x.rowguid = t.rowguid

drop table #tmpTrx