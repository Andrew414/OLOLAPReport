SELECT 
	Buyer.Gender,
	Store.Address,
	sum(Operation.Price)
FROM
	Operation
	INNER JOIN Buyer ON Buyer.Id = Operation.BuyerId
	INNER JOIN Store ON Store.Id = Operation.StoreId
	INNER JOIN Item  ON Item.Id  = Operation.ItemId
WHERE
	Operation.ItemId in
		(SELECT Id FROM Item WHERE Item.Model like 'iPhone 5s')
GROUP BY
	Buyer.Gender,
	Store.Address;


SELECT 
	strftime("%m-%Y", Operation.Date) as "month",
	Store.Address,
	count(Operation.Amount)
FROM
	Operation
	INNER JOIN Buyer ON Buyer.Id = Operation.BuyerId
	INNER JOIN Store ON Store.Id = Operation.StoreId
	INNER JOIN Item  ON Item.Id  = Operation.ItemId

GROUP BY
	strftime("%m-%Y", Operation.Date),
	Store.Address;
