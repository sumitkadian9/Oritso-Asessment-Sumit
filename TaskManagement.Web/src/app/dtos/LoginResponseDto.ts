import { UserDto } from "./UserDto";

export interface LoginResponseDto extends UserDto {
    token: string;
}