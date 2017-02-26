SELECT P.ProdNo, ProdName, Qty, ProdPrice
FROM Customer C
INNER JOIN OrderTbl OTbl
ON C.CustNo = OTbl.CustNo
INNER JOIN OrdLine OL
ON OTbl.OrdNo = OL.OrdNo
INNER JOIN Product P
ON OL.ProdNo = P.ProdNo
WHERE OTbl.OrdNo = 'O5511365' AND C.CustNo = 'C3340959';