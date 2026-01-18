import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AccountService } from '../../services/account-service';
import { RegisterDto } from '../../dtos/RegisterDto';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css',
  standalone: true,
})
export class Register {
  private fb = inject(FormBuilder);
  private accountService = inject(AccountService);
  private router = inject(Router);

  errorMessage: string | null = null;
  
  registerForm: FormGroup = this.fb.group({
    firstName: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  onSubmit() {
    if (this.registerForm.valid) {
      const registerDto: RegisterDto = this.registerForm.value;
      
      this.accountService.register(registerDto).subscribe({
        next: () => {
          this.router.navigate(['/login']);
        },
        error: (err) => {
          this.errorMessage = err.error?.detail || 'Registration failed. Please try again.';
        }
      });
    }
  }
}
