import {Component, inject, OnInit} from '@angular/core';
import {
  CreateCustomEventCategoryModel,
  CustomEventCategoryModel,
  UpdateCustomEventCategoryModel
} from "../../../models/customEventCategory.model";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {JwtTokenService} from "../../../services/jwt-token.service";
import {CustomEventCategoryService} from "../../../services/custom-event-category.service";
import {ToastrService} from "ngx-toastr";
import Swal from "sweetalert2";

@Component({
  selector: 'app-custom-event-category',
  templateUrl: './custom-event-category.component.html',
  styleUrl: './custom-event-category.component.css'
})
export class CustomEventCategoryComponent implements OnInit {
  isCreateCustomEventCategory: any;
  customEventCategories: CustomEventCategoryModel[] = [];
  customEventCategoryForm!: FormGroup;
  isUpdate: boolean = false;
  private readonly _tokenService: JwtTokenService = inject(JwtTokenService);
  private readonly _customEventCategoryService: CustomEventCategoryService = inject(CustomEventCategoryService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private allianceId: string = this._tokenService.getAllianceId()!;
  pageNumber: number = 1;

  get f() {
    return this.customEventCategoryForm.controls;
  }

  ngOnInit() {
    this.getCustomEventCategories();
  }

  getCustomEventCategories() {
    this._customEventCategoryService.getAllianceCustomEventCategories(this.allianceId).subscribe({
      next: ((response) => {
        if (response) {
          this.customEventCategories = response;
        } else {
          this.customEventCategories = [];
        }
      }),
      error: (error) => {
        console.error(error);
      }
    });
  }

  onCreateCategory() {
    this.createCustomEventCategoryForm(false);
  }

  createCustomEventCategoryForm(isUpdate: boolean, customEventCategory: CustomEventCategoryModel | null = null) {
    this.customEventCategoryForm = new FormGroup({
      id: new FormControl<string>(isUpdate ? customEventCategory!.id : ''),
      allianceId: new FormControl<string>(this.allianceId),
      name: new FormControl<string>(isUpdate ? customEventCategory!.name : '', [Validators.required, Validators.maxLength(150)]),
      isPointsEvent: new FormControl<boolean>(isUpdate ? customEventCategory!.isPointsEvent : false),
      isParticipationEvent: new FormControl(isUpdate ? customEventCategory!.isParticipationEvent : false),
    });
    if (isUpdate) {
      this.customEventCategoryForm.controls['isPointsEvent'].disable();
      this.customEventCategoryForm.controls['isParticipationEvent'].disable();
    }
    this.isCreateCustomEventCategory = true;
  }

  onCancel() {
    this.isUpdate = false;
    this.isCreateCustomEventCategory = false;
  }

  onEditCustomEventCategory(customEventCategory: CustomEventCategoryModel) {
    this.createCustomEventCategoryForm(true, customEventCategory);
  }

  onDeleteCustomEventCategory(customEventCategory: CustomEventCategoryModel) {
    Swal.fire({
      title: `Delete category: ${customEventCategory.name}`,
      text: `Do you really want to delete the category? This will also delete all events associated with this category.`,
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!"
    }).then((result) => {
      if (result.isConfirmed) {
        this._customEventCategoryService.deleteCustomEventCategory(customEventCategory.id).subscribe({
          next: ((response) => {
            if (response) {
              Swal.fire({
                title: "Deleted!",
                text: "Custom event has been deleted",
                icon: "success"
              }).then(_ => this.resetAndGetCustomEventCategories());
            }
          }),
          error: (error: Error) => {
            console.log(error);
          }
        });
      }
    });
  }

  onSubmit() {
    if (this.customEventCategoryForm.invalid) {
      return;
    }
    if (this.isUpdate) {
      this.updateCustomEventCategory();
      return;
    }
    const customEventCategory: CreateCustomEventCategoryModel = this.customEventCategoryForm.value as CreateCustomEventCategoryModel;
    this._customEventCategoryService.createCustomEventCategory(customEventCategory).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Successfully created!', 'Successfully');
          this.onCancel();
          this.resetAndGetCustomEventCategories();
        }
      }),
      error: ((error: any) => {
        console.error(error);
      })
    })
  }

  pageChanged(event: number) {
    this.pageNumber = event;
  }

  updateCustomEventCategory() {
    const customEventCategory: UpdateCustomEventCategoryModel = this.customEventCategoryForm.value as UpdateCustomEventCategoryModel;
    this._customEventCategoryService.updateCustomEvent(customEventCategory.id, customEventCategory).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Successfully updated!', 'Successfully');
          this.onCancel();
          this.resetAndGetCustomEventCategories();
        }
      }),
      error: ((error: any) => {
        console.error(error);
      })
    })
  }

  resetAndGetCustomEventCategories() {
    this.pageNumber = 1;
    this.getCustomEventCategories();
  }
}
