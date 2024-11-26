export interface LoginRequestModel {
  email: string;
  password: string;
}

export interface LoginResponseModel {
  token: string;
}
