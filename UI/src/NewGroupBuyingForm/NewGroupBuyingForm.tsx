import * as React from "react";
import * as FormsStyles from "../FormStyles/FormsStyles";
import { Stack, Dropdown } from "office-ui-fabric-react";
import {
  DatePicker,
  DayOfWeek,
  DefaultButton,
  IDatePickerStrings,
  IDropdownOption,
  PrimaryButton,
  Separator,
  Text,
  TextField,
  Image,
  MessageBar,
  MessageBarType,
  Spinner,
} from "@fluentui/react";
import { CategoriesMap } from "../HomePage/Model/Categories";
import { newProductRequest } from "../Modal/ProductDetails";
import { NewBidRequest } from "../Modal/GroupDetails";
import { submitNewGroupForm } from "../Services/BidsControllerService";
import { useHistory } from "react-router";
import ButtonAppBar from "../LoginBar";
import { useAuth0 } from "@auth0/auth0-react";

export const NewGroupBuyingForm: React.FunctionComponent = () => {
  const [productDetails, setProductDetails] = React.useReducer<
    (
      prevState: Partial<newProductRequest>,
      state: Partial<newProductRequest>
    ) => newProductRequest
  >(
    (prevState: any, state: any) => ({
      ...prevState,
      ...state,
    }),
    {
      name: "",
      image: undefined,
      description: "",
    }
  );

  const [bidRequest, setBidRequest] = React.useReducer<
    (
      prevState: Partial<NewBidRequest>,
      state: Partial<NewBidRequest>
    ) => NewBidRequest
  >(
    (prevState: any, state: any) => ({
      ...prevState,
      ...state,
    }),
    {
      ownerId: "1",
      category: "",
      subCategory: "",
      // Tomorrow Date
      expirationDate: new Date(new Date().setDate(new Date().getDate() + 1)),
      maxPrice: 0,
      product: undefined,
    }
  );

  const [
    allRequiredFieldsAreFulfilled,
    SetAllRequiredFieldsAreFulfilled,
  ] = React.useState<boolean>(false);

  const [requestInProcess, setRequestInProcess] = React.useState<boolean>(
    false
  );

  const [errorMessage, setErrorMessage] = React.useState<string>();
  const urlHistory = useHistory();

  React.useEffect(() => {
    const allRequiredFieldsAreFulfilledObjectTemp: boolean =
      Object.values(bidRequest).every((el) => el) &&
      !!productDetails.description &&
      !!productDetails.name;

    SetAllRequiredFieldsAreFulfilled(allRequiredFieldsAreFulfilledObjectTemp);
  }, [bidRequest, productDetails]);

  React.useEffect(() => {
    setBidRequest({ product: productDetails });
  }, [productDetails]);

  function onDropdownChange(
    event: React.FormEvent<HTMLDivElement>,
    option?: IDropdownOption
  ): void {
    const id: string | undefined = option?.id as string;
    const text: string | undefined = option?.text!;
    setBidRequest({ [id]: text });
  }

  const onTextFieldChange = (
    setStateFunction: Function,
    event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>,
    newValue?: string,
    convertToNumber?: boolean
  ): void => {
    const updatedValue: string | number | undefined = convertToNumber
      ? Number(newValue)
      : newValue;

    setStateFunction({
      [(event.target as HTMLInputElement).id]: updatedValue,
    });
  };

  const { getAccessTokenSilently } = useAuth0();

  const onSubmitForm = async (): Promise<void> => {
    try {
      setRequestInProcess(true);
      await submitNewGroupForm(bidRequest, getAccessTokenSilently);
      urlHistory.push(`/`);
    } catch {
      setRequestInProcess(false);
      setErrorMessage(
        "An error occurred while trying to create your group. Please try again later."
      );
    }
  };

  return (
    <Stack horizontalAlign={"center"}>
      <ButtonAppBar />
      <Text
        block={true}
        className="Bold"
        styles={FormsStyles.headerStyle}
        variant="xLargePlus"
      >
        New suggestion for Group buying
      </Text>
      <Stack
        styles={{ root: { width: "40%" } }}
        tokens={FormsStyles.verticalGapStackTokens}
      >
        <Separator styles={{ root: { width: "100%" } }} />
        {errorMessage && (
          <MessageBar
            messageBarType={MessageBarType.error}
            onDismiss={() => setErrorMessage("")}
          >
            {errorMessage}
          </MessageBar>
        )}
        <TextField
          id="name"
          label="Product's name"
          styles={{ root: { width: FormsStyles.inputWidth } }}
          onChange={(event, newValue) => {
            onTextFieldChange(setProductDetails, event, newValue);
          }}
          required
        />
        <Dropdown
          id="category"
          onChange={onDropdownChange}
          label={"Product's Category"}
          styles={FormsStyles.dropdownStyles}
          options={Array.from(CategoriesMap.keys()).map((categoryName) => ({
            key: categoryName,
            id: "category",
            text: categoryName,
          }))}
          required
        ></Dropdown>
        <Dropdown
          id="subCategory"
          label={"Product's Sub Category"}
          onChange={onDropdownChange}
          styles={FormsStyles.dropdownStyles}
          options={
            bidRequest.category
              ? Array.from(
                  CategoriesMap.get(bidRequest.category)!.map(
                    (subCategory) => ({
                      key: subCategory,
                      id: "subCategory",
                      text: subCategory,
                    })
                  )
                )
              : []
          }
          required
        ></Dropdown>
        <TextField
          id="maxPrice"
          onChange={(event, newValue) =>
            onTextFieldChange(setBidRequest, event, newValue)
          }
          label="Maximum price"
          onGetErrorMessage={(value) =>
            value === "" || value.match(/^[0-9]+$/) != null
              ? ""
              : "Only numbers allowed"
          }
          styles={{ root: { width: FormsStyles.inputWidth } }}
          suffix="₪"
          required
        />
        <DatePicker
          id="expirationDate"
          onSelectDate={(date: Date | null | undefined): void => {
            setBidRequest({
              expirationDate: date!,
            });
          }}
          value={bidRequest.expirationDate ?? undefined}
          label="Set expiration date for the group"
          firstDayOfWeek={DayOfWeek.Sunday}
          strings={DayPickerStrings}
          placeholder="Select a date"
          styles={{ root: { width: FormsStyles.inputWidth } }}
          isRequired
          // Tomorrow Date
          minDate={new Date(new Date().setDate(new Date().getDate() + 1))}
        />
        <TextField
          id="description"
          onChange={(event, newValue) => {
            onTextFieldChange(setProductDetails, event, newValue);
          }}
          label="Description"
          multiline
          autoAdjustHeight
          styles={{ root: { width: FormsStyles.inputWidth } }}
          required
        />
        <TextField
          id="image"
          label="Enter URL image for reference"
          styles={{ root: { width: FormsStyles.inputWidth } }}
          onChange={(event, newValue) => {
            onTextFieldChange(setProductDetails, event, newValue);
          }}
        />
        {productDetails?.image && (
          <Image
            src={productDetails.image}
            id="target"
            width={"25rem"}
            height={"20rem"}
          />
        )}
        <Separator styles={{ root: { width: FormsStyles.inputWidth } }} />
        <Stack horizontal horizontalAlign={"space-between"}>
          <DefaultButton text="Cancel" href={"/"} />
          <Stack horizontal tokens={FormsStyles.verticalGapStackTokens}>
            {requestInProcess && <Spinner />}
            <PrimaryButton
              text="Send"
              onClick={() => {
                onSubmitForm();
              }}
              disabled={!allRequiredFieldsAreFulfilled}
            />
          </Stack>
        </Stack>
      </Stack>
    </Stack>
  );
};

const DayPickerStrings: IDatePickerStrings = {
  months: [
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December",
  ],

  shortMonths: [
    "Jan",
    "Feb",
    "Mar",
    "Apr",
    "May",
    "Jun",
    "Jul",
    "Aug",
    "Sep",
    "Oct",
    "Nov",
    "Dec",
  ],

  days: [
    "Sunday",
    "Monday",
    "Tuesday",
    "Wednesday",
    "Thursday",
    "Friday",
    "Saturday",
  ],

  shortDays: ["S", "M", "T", "W", "T", "F", "S"],

  goToToday: "Go to today",
  prevMonthAriaLabel: "Go to previous month",
  nextMonthAriaLabel: "Go to next month",
  prevYearAriaLabel: "Go to previous year",
  nextYearAriaLabel: "Go to next year",
  closeButtonAriaLabel: "Close date picker",
  monthPickerHeaderAriaLabel: "{0}, select to change the year",
  yearPickerHeaderAriaLabel: "{0}, select to change the month",
};
