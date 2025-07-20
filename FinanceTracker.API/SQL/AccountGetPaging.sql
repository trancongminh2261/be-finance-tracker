select  ac.id, ac.Username, aci.Fullname, aci.Phone, aci.Email, ac.Status,
aci.Address, ac.created, ac.created_by, ac.updated, ac.updated_by,
count(ac.id) over() as TotalItem
from Account ac join AccountInfo aci on ac.id = aci.Accountid
order by ac.created desc 
OFFSET @PageIndex ROWS FETCH NEXT @PageSize ROWS ONLY