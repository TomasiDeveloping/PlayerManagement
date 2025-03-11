import {Component, inject, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {environment} from "../../../environments/environment";
import {FeedbackService} from "../../services/feedback.service";
import {ToastrService} from "ngx-toastr";
import {Router} from "@angular/router";

@Component({
  selector: 'app-feedback',
  templateUrl: './feedback.component.html',
  styleUrl: './feedback.component.css'
})
export class FeedbackComponent implements OnInit {

  private readonly _fb: FormBuilder = inject(FormBuilder);
  private readonly _feedbackService: FeedbackService = inject(FeedbackService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _router: Router = inject(Router);

  public feedbackForm!: FormGroup;
  public feedbackType: string = 'feature';
  public isSubmitted: boolean = false;
  public issueUrl: string = '';

  private appVersion: string = environment.version;

  get f() {
    return this.feedbackForm.controls;
  }

  ngOnInit() {
    this.createForm();
  }

  createForm() {
    this.feedbackForm = this._fb.group({
      type: [this.feedbackType, Validators.required],
      title: ['', Validators.required],
      description: ['', Validators.required],
      appVersion: [this.appVersion],
      email: ['', Validators.email],
      screenshot: [null]
    });
  }

  onTypeChange(event: any) {
    const type = event.target.value;
    this.feedbackType = type;
    if (type === 'bug') {
      this.feedbackForm.addControl('expectedBehavior', this._fb.control('', Validators.required));
      this.feedbackForm.addControl('actualBehavior', this._fb.control('', Validators.required));
      this.feedbackForm.addControl('reproduction', this._fb.control('', Validators.required));
      this.feedbackForm.addControl('severity', this._fb.control('medium', Validators.required));
      this.feedbackForm.addControl('os', this._fb.control('', Validators.required));
    } else {
      this.feedbackForm.removeControl('expectedBehavior');
      this.feedbackForm.removeControl('actualBehavior');
      this.feedbackForm.removeControl('reproduction');
      this.feedbackForm.removeControl('severity');
      this.feedbackForm.removeControl('os');
    }
  }

  onSubmit() {
    if (this.feedbackForm.valid) {
      const formData = new FormData();
      Object.keys(this.feedbackForm.value).forEach((key) => {
        formData.append(key, this.feedbackForm.value[key]);
      });

      if (this.feedbackForm.get('screenshot')?.value) {
        formData.append('screenshot', this.feedbackForm.get('screenshot')?.value);
      }

      this._feedbackService.submitFeedback(formData).subscribe({
        next: ((response) => {
          if (response) {
            this.isSubmitted = true;
            this.issueUrl = response.url;
          } else {
            this._toastr.error('Failure submitted feedback', 'Feedback');
          }
        }),
        error: (error) => {
          console.log(error);
          this._toastr.error('Failure submitted feedback', 'Feedback');
        }
      });
    }
  }

  onFileChange(event: any) {
    if (event.target.files && event.target.files.length > 0) {
      this.feedbackForm.patchValue({ screenshot: event.target.files[0] });
    }
  }

  onCancel() {
    this._router.navigate(['/']).then();
  }
}
