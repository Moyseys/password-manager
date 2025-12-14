export interface SecretListInterface {
  id: string;
  title: string;
  username: string;
  password: string;
  audit: {
    createdAt?: string;
    createdBy?: string;
    updatedAt?: string;
    updatedBy?: string;
  };
}
