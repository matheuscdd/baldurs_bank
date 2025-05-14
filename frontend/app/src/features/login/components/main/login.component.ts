import { Component, inject, Input } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CheckboxModule } from 'primeng/checkbox';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { TextareaModule } from 'primeng/textarea';
import { FluidModule } from 'primeng/fluid';
import { InputIconModule } from 'primeng/inputicon';
import { IftaLabelModule } from 'primeng/iftalabel';
import { PasswordModule } from 'primeng/password';
import { DividerModule } from 'primeng/divider';
import { MessageService } from 'primeng/api';
import { Toast } from 'primeng/toast';
import { LoginService } from '../../services/login.service';
import { tUserLogin, userSchemaLogin } from '../../schemas/login.schema';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  standalone: true,
  selector: 'app-login',
  imports: [
    FormsModule, 
    InputTextModule, 
    FluidModule, 
    ButtonModule, 
    SelectModule, 
    TextareaModule,
    InputIconModule,
    IftaLabelModule,
    PasswordModule,
    DividerModule,
    Toast,
  ],
  providers: [LoginService, MessageService],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  @Input() title!: string;
  @Input() isManager!: boolean;
  @Input() destination!: string;
  blockBtn = false;
  email!: string;
  password!: string;
  zodErrors: { [key: string]: string } = {};
  private readonly form: FormGroup;

  private readonly loginService = inject(LoginService);
  private readonly router = inject(Router);
  private readonly messageService = inject(MessageService);
  private readonly authService = inject(AuthService);

  constructor(private readonly fb: FormBuilder) {
    this.form = this.fb.group({
      email: [String()],
      password: [String()]
    });
  }

  onSubmit() {
    const result = userSchemaLogin.safeParse({
          email: this.email,
          password: this.password,
        } as tUserLogin);
    this.zodErrors = {};

    if (!result.success) {
      result.error.errors.reverse().forEach(err => {
        this.zodErrors[String(err.path[0])] = err.message
      });

      return;
    }

    this.blockBtn = true;
    this.loginService.login(result.data, this.isManager).subscribe({
      next: () => {
        this.blockBtn = false;
        this.router.navigate([this.destination]);
      },
      error: ({error}) => {
        this.blockBtn = false;
        const timer = 3000;
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Incorrect Credentials', life: timer });
        setTimeout(() => this.authService.logout, timer);
      }
    });
  }
}
