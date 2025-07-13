select  ac.Id, ac.Username, aci.Fullname, aci.Phone, aci.Email, ac.Status,
aci.Address, ac.Created, ac.CreatedBy, ac.Updated, ac.UpdatedBy,
count(ac.Id) over() as TotalItem
from Account ac join AccountInfo aci on ac.Id = aci.AccountId
order by ac.Created desc 
OFFSET @PageIndex ROWS FETCH NEXT @PageSize ROWS ONLY