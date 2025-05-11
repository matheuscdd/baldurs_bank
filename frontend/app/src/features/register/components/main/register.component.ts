import { Component, inject } from '@angular/core';
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
import { RegisterService } from '../../services/register.service';
import { tUserRegister, userSchemaRegister } from '../../schemas/register.schema';


@Component({
  standalone: true,
  selector: 'app-register',
  imports: [
    FormsModule, 
    CheckboxModule, 
    InputTextModule, 
    FluidModule, 
    ButtonModule, 
    IftaLabelModule,
    PasswordModule,
    DividerModule,
    Toast,
  ],
  providers: [RegisterService, MessageService],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  blockBtn = false;
  isManager: boolean = false;
  name!: string;
  email!: string;
  password!: string;
  confirmPassword!: string;
  zodErrors: { [key: string]: string } = {};

  private readonly registerService = inject(RegisterService);
  private readonly router = inject(Router);
  private readonly messageService = inject(MessageService);
  private readonly form: FormGroup;

  constructor(private readonly fb: FormBuilder) {
    this.form = this.fb.group({
      name: [String()],
      email: [String()],
      password: [String()]
    })
  }

  onSubmit() {
    const result = userSchemaRegister.safeParse({
      name: this.name,
      email: this.email,
      password: this.password,
      confirmPassword: this.confirmPassword,
      isManager: this.isManager,
    } as tUserRegister);
    this.zodErrors = {};

    if (!result.success) {
      result.error.errors.reverse().forEach(err => {
        this.zodErrors[String(err.path[0])] = err.message;
      });

      return;
    }

    this.blockBtn = true;
    this.registerService.register(result.data).subscribe({
      next: () => {
        this.blockBtn = false;
        const timer = 3000;
        this.messageService.add({ severity: 'success', summary: 'User Created', detail: 'Welcome to our bank', life: timer });
        setTimeout(() => this.router.navigate([this.isManager ? '/dashboard' : '/home']), timer);
      },
      error: ({error}) => {
        this.blockBtn = false;
        this.messageService.add({ severity: 'error', summary: 'Error', detail: error.title, life: 7000 });
      }
    });
  }
}
