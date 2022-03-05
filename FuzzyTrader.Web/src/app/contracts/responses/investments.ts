export interface InvestmentOptions {
  productId: string;
  description: string;
  baseValue?: number;
  totalValue?: number;
  dailyTradedVolume?: number;
  quantity: number;
  isCrypto: boolean;
}
