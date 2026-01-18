import { Role } from "../enums/Role";

export interface UserDto
{
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    role: Role
}