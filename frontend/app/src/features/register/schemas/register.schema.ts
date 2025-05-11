import { z } from 'zod';

export const userSchemaRegister = z.object({
    name: z.string().trim().min(6).max(120),
    email: z.string().trim().email().min(5).max(100),
    isManager: z.boolean(),
    password: z.string().trim()
        .min(12)
        .regex(/[A-Z]/, 'At least one uppercase')
        .regex(/[a-z]/, 'At least one lowercase')
        .regex(/\d/, 'At least one numeric')
        .regex(/[^a-zA-Z0-9-]/, 'At least one non alphanumeric'),
    confirmPassword: z.string().trim()
}).refine(data => data.password === data.confirmPassword, {
    message: 'Passwords do not match',
    path: ['confirmPassword'],
});


export type tUserRegister = z.infer<typeof userSchemaRegister>;
