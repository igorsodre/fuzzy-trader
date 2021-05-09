export interface AppMessage {
  text: string;
  type: 'INFO' | 'WARN' | 'ERROR' | 'SUCCESS';
}
