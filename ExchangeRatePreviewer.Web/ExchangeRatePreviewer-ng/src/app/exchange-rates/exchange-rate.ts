export interface ExchangeRate {
  currency?: string,
  date: string,
  quantity: number,
  unitDescription?: string,
  rate: number,
  rateChangeVsPreviousDay: number
}
