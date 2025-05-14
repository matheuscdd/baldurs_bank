export type tTransaction = {
	Id: string,
	AccountId: string,
	Value: 200,
	Method: "Credit" | "Debit",
	CreatedAt: string
}